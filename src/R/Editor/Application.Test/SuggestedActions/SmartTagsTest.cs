﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Languages.Editor.Shell;
using Microsoft.R.Components.ContentTypes;
using Microsoft.R.Editor.SuggestedActions.Actions;
using Microsoft.UnitTests.Core.Mef;
using Microsoft.UnitTests.Core.XUnit;
using Microsoft.VisualStudio.Language.Intellisense;
using Xunit;

namespace Microsoft.R.Editor.Application.Test.SuggestedActions {
    [ExcludeFromCodeCoverage]
    [Collection(CollectionNames.NonParallel)]
    public class SmartTagsTest : IDisposable {
        private readonly IExportProvider _exportProvider;
        private readonly EditorHostMethodFixture _editorHost;

        public SmartTagsTest(REditorApplicationMefCatalogFixture catalogFixture, EditorHostMethodFixture editorHost) {
            _exportProvider = catalogFixture.CreateExportProvider();
            _editorHost = editorHost;
        }

        public void Dispose() {
            _exportProvider.Dispose();
        }
        
        [Test]
        [Category.Interactive]
        public async Task R_LibrarySuggestedActions() {
            using (var script = await _editorHost.StartScript(_exportProvider, " library(base)", RContentTypeDefinition.ContentType)) {
                IEnumerable<SuggestedActionSet> sets = null;
                ILightBulbSession session = null;

                var svc = _exportProvider.GetExportedValue<ISuggestedActionCategoryRegistryService>();

                script.Invoke(() => {
                    var broker = _exportProvider.GetExportedValue<ILightBulbBroker>();
                    broker.CreateSession(svc.AllCodeFixes, script.View);
                    session = script.GetLightBulbSession();
                    session.Should().NotBeNull();
                    session.Expand();
                    session.TryGetSuggestedActionSets(out sets);
                });

                sets.Should().NotBeNull();
                sets.Should().HaveCount(0);
                session.Dismiss();
                script.DoIdle(200);

                script.MoveRight(2);
                script.DoIdle(200);

                sets = null;
                script.Invoke(() => {
                    var broker = _exportProvider.GetExportedValue<ILightBulbBroker>();
                    broker.DismissSession(script.View);
                    broker.CreateSession(svc.Any, script.View);
                    session = script.GetLightBulbSession();
                    session.Should().NotBeNull();
                    session.Expand();
                    session.TryGetSuggestedActionSets(out sets);
                });
                script.DoIdle(3000);

                sets = null;
                script.Invoke(() => {
                    session.TryGetSuggestedActionSets(out sets);
                });

                sets.Should().NotBeNull();
                sets.Should().HaveCount(1);

                var set = sets.First();
                set.Actions.Should().HaveCount(2);
                var actions = set.Actions.ToArray();
                actions[0].Should().BeOfType(typeof(InstallPackageSuggestedAction));
                actions[1].Should().BeOfType(typeof(LoadLibrarySuggestedAction));
            }
        }
    }
}
