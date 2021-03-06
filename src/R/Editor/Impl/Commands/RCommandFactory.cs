﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Languages.Editor.BraceMatch;
using Microsoft.Languages.Editor.Controller;
using Microsoft.Languages.Editor.Shell;
using Microsoft.R.Components.ContentTypes;
using Microsoft.R.Components.InteractiveWorkflow;
using Microsoft.R.Editor.Comments;
using Microsoft.R.Editor.Completion.Documentation;
using Microsoft.R.Editor.Formatting;
using Microsoft.R.Editor.Navigation.Commands;
using Microsoft.R.Editor.Selection;
using Microsoft.R.Host.Client;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.R.Editor.Commands {
    [Export(typeof(ICommandFactory))]
    [ContentType(RContentTypeDefinition.ContentType)]
    internal class RCommandFactory : ICommandFactory {
        private readonly IObjectViewer _objectViewer;
        private readonly IRInteractiveWorkflowProvider _workflowProvider;
        private readonly IEditorShell _editorShell;

        [ImportingConstructor]
        public RCommandFactory([Import(AllowDefault = true)] IObjectViewer objectViewer, [Import(AllowDefault = true)] IRInteractiveWorkflowProvider workflowProvider, IEditorShell editorShell) {
            _objectViewer = objectViewer;
            _workflowProvider = workflowProvider;
            _editorShell = editorShell;
        }

        public IEnumerable<ICommand> GetCommands(ITextView textView, ITextBuffer textBuffer) {
            var commands = new List<ICommand> {
                new GotoBraceCommand(textView, textBuffer, _editorShell),
                new CommentCommand(textView, textBuffer, _editorShell),
                new UncommentCommand(textView, textBuffer, _editorShell),
                new FormatDocumentCommand(textView, textBuffer, _editorShell),
                new FormatSelectionCommand(textView, textBuffer, _editorShell),
                new FormatOnPasteCommand(textView, textBuffer, _editorShell),
                new SelectWordCommand(textView, textBuffer),
                new RTypingCommandHandler(textView, _editorShell),
                new RCompletionCommandHandler(textView),
                new PeekDefinitionCommand(textView, textBuffer, _editorShell.ExportProvider.GetExportedValue<IPeekBroker>()),
                new InsertRoxygenBlockCommand(textView, textBuffer)
            };

            if (_workflowProvider != null) {
                commands.Add(new GoToDefinitionCommand(textView, textBuffer, _objectViewer, _workflowProvider.GetOrCreate().RSession));
            }

            return commands;
        }
    }
}
