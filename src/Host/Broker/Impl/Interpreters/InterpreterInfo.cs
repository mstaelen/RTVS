﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.R.Host.Broker.Interpreters {
    public class InterpreterInfo {
        public string Name { get; set; }

        public string Path { get; set; }

        public string BinPath { get; set; }

        [JsonConverter(typeof(VersionConverter))]
        public Version Version { get; set; }
    }
}
