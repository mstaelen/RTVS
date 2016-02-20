﻿using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Common.Core.Test.Controls;
using Microsoft.R.Components.ContentTypes;
using Microsoft.R.Editor.ContentType;
using Microsoft.R.Host.Client;
using Microsoft.R.Host.Client.Test.Script;
using Microsoft.UnitTests.Core.XUnit;
using Microsoft.VisualStudio.R.Package.Help;
using Microsoft.VisualStudio.R.Package.Test;
using Microsoft.VisualStudio.R.Package.Shell;
using Microsoft.VisualStudio.R.Package.Test.Mocks;
using Microsoft.VisualStudio.Text;
using Xunit;

namespace Microsoft.VisualStudio.R.Interactive.Test.Help {
    [ExcludeFromCodeCoverage]
    [Collection(CollectionNames.NonParallel)]
    public class HelpOnCurrentTest : InteractiveTest {
        [Test]
        [Category.Interactive]
        public void HelpTest() {
            var clientApp = new RHostClientHelpTestApp();
            var sessionProvider = VsAppShell.Current.ExportProvider.GetExportedValue<IRSessionProvider>();
            using (var hostScript = new RHostScript(sessionProvider, clientApp)) {
                using (var script = new ControlTestScript(typeof(HelpWindowVisualComponent))) {
                    DoIdle(100);

                    var component = ControlWindow.Component as IHelpWindowVisualComponent;
                    component.Should().NotBeNull();

                    component.VisualTheme = "Light.css";
                    clientApp.Component = component;

                    var viewTracker = new ActiveTextViewTrackerMock("  plot", RContentTypeDefinition.ContentType);
                    var view = viewTracker.GetLastActiveTextView(RContentTypeDefinition.ContentType);
                    var cmd = new ShowHelpOnCurrentCommand(hostScript.Session, viewTracker);

                    cmd.Should().BeVisibleAndDisabled();

                    view.Caret.MoveTo(new SnapshotPoint(view.TextBuffer.CurrentSnapshot, 3));

                    cmd.Should().BeVisibleAndEnabled();
                    cmd.Text.Should().EndWith("plot");

                    cmd.Invoke();
                    WaitForAppReady(clientApp);

                    clientApp.Uri.IsLoopback.Should().Be(true);
                    clientApp.Uri.PathAndQuery.Should().Be("/library/graphics/html/plot.html");

                    DoIdle(500);
                }
            }
        }

        private void WaitForAppReady(RHostClientHelpTestApp clientApp) {
            for (int i = 0; i < 100 && !clientApp.Ready; i++) {
                DoIdle(200);
            }
        }
    }
}