﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Microsoft.R.Components.Extensions;
using Microsoft.R.Debugger;
using Microsoft.VisualStudio.R.Package.Shell;

namespace Microsoft.VisualStudio.R.Package.DataInspect.Viewers {
    public abstract class ViewerBase {
        protected IDataObjectEvaluator Evaluator { get; }

        public ViewerBase(IDataObjectEvaluator evaluator) {
             Evaluator = evaluator;
        }

        protected async Task<DebugValueEvaluationResult> EvaluateAsync(string expression, DebugEvaluationResultFields fields) {
            var result = await Evaluator.EvaluateAsync(expression, fields);
            var error = result as DebugErrorEvaluationResult;
            if (error != null) {
                await VsAppShell.Current.SwitchToMainThreadAsync();
                VsAppShell.Current.ShowErrorMessage(error.ErrorText);
                return null;
            }

            return result as DebugValueEvaluationResult;
        }
    }
}
