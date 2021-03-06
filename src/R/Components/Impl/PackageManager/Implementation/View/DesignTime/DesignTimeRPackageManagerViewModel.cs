﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.R.Components.PackageManager.ViewModel;

namespace Microsoft.R.Components.PackageManager.Implementation.View.DesignTime {
#if DEBUG
    internal class DesignTimeRPackageManagerViewModel : IRPackageManagerViewModel {
        public ReadOnlyObservableCollection<object> Items { get; } = new ReadOnlyObservableCollection<object>(new ObservableCollection<object> {
            new DesignTimeRPackageViewModel("rtvs1", "0.3", "0.3", "abc.data, nnet, quantreg, MASS, locfit", "GPL (>= 3)", true, false),
            new DesignTimeRPackageViewModel("rtvs2", "1.0.4", "1.0.0", "abc, abind, parallel, plyr", "GPL (>= 2)", true, true),
            new DesignTimeRPackageViewModel("rtvs3", "2.1.0", null, "digest, grid, gtable (>= 0.1.1), MASS, plyr (>= 1.7.1), reshape2, scales (>= 0.3.0), stats", "GPL-2", false, false)
        });

        public IRPackageViewModel SelectedPackage => (IRPackageViewModel)Items[1];
        public bool IsLoading => false;
        public bool ShowPackageManagerDisclaimer { get; set; } = true;

        public string FirstError => null;
        public bool HasMultipleErrors => false;


        public Task SwitchToAvailablePackagesAsync() => Task.CompletedTask;
        public Task SwitchToInstalledPackagesAsync() => Task.CompletedTask;
        public Task SwitchToLoadedPackagesAsync() => Task.CompletedTask;

        public void ReloadItems() {
            
        }

        public void SelectPackage(IRPackageViewModel package) {
            
        }

        public Task InstallAsync(IRPackageViewModel package) => Task.CompletedTask;
        public Task UpdateAsync(IRPackageViewModel package) => Task.CompletedTask;
        public Task UninstallAsync(IRPackageViewModel package) => Task.CompletedTask;
        public Task LoadAsync(IRPackageViewModel package) => Task.CompletedTask;
        public Task UnloadAsync(IRPackageViewModel package) => Task.CompletedTask;
        public Task DefaultActionAsync() => Task.CompletedTask;

        public void DismissErrorMessage() {
        }

        public void DismissAllErrorMessages() {
        }

        public Task<int> Search(string searchString, CancellationToken cancellationToken) {
            return Task.FromResult(0);
        }

        public void Dispose() {
        }
    }
#endif
}
