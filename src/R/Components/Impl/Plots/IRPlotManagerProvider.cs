﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.R.Components.InteractiveWorkflow;
using Microsoft.R.Components.Settings;
using Microsoft.R.Host.Client;

namespace Microsoft.R.Components.Plots {
    public interface IRPlotManagerProvider {
        IRPlotManager CreatePlotManager(IRSessionProvider sessionProvider, IRSettings settings, IRInteractiveWorkflow interactiveWorkflow);
    }
}
