﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.UnitTests.Core.XUnit;
using Microsoft.VisualStudio.R.Package.Commands;
using Microsoft.VisualStudio.R.Package.Plots;
using Microsoft.VisualStudio.R.Package.Plots.Commands;
using Microsoft.VisualStudio.R.Package.Plots.Definitions;
using NSubstitute;

namespace Microsoft.VisualStudio.R.Package.Test.Plots {
    [ExcludeFromCodeCoverage]
    public class PlotCommandsTest {
        [Test]
        [Category.Plots]
        public void HistoryNext() {
            var history = new PlotHistory();
            var cmd = new HistoryNextPlotCommand(history);

            cmd.CommandID.ID.Should().Be(RPackageCommandId.icmdNextPlot);
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 1;
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 2;
            cmd.Should().BeEnabled();

            history.ActivePlotIndex = 1;
            history.PlotCount = 2;
            cmd.Should().BeDisabled();
        }

        [Test]
        [Category.Plots]
        public void HistoryPrevious() {
            var history = new PlotHistory();
            var cmd = new HistoryPreviousPlotCommand(history);

            cmd.CommandID.ID.Should().Be(RPackageCommandId.icmdPrevPlot);
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 1;
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 2;
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 1;
            history.PlotCount = 2;
            cmd.Should().BeEnabled();
        }

        [Test]
        [Category.Plots]
        public void CopyPlotAsBitmap() {
            var history = new PlotHistory();
            var cmd = new CopyPlotAsBitmapCommand(history);

            cmd.CommandID.ID.Should().Be(RPackageCommandId.icmdCopyPlotAsBitmap);
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 1;
            cmd.Should().BeEnabled();
        }

        [Test]
        [Category.Plots]
        public void CopyPlotAsMetafile() {
            var history = new PlotHistory();
            var cmd = new CopyPlotAsMetafileCommand(history);

            cmd.CommandID.ID.Should().Be(RPackageCommandId.icmdCopyPlotAsMetafile);
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 1;
            cmd.Should().BeEnabled();
        }

        [Test]
        [Category.Plots]
        public void ExportPlotAsImage() {
            var history = new PlotHistory();
            var cmd = new ExportPlotAsImageCommand(history);

            cmd.CommandID.ID.Should().Be(RPackageCommandId.icmdExportPlotAsImage);
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 1;
            cmd.Should().BeEnabled();
        }

        [Test]
        [Category.Plots]
        public void ExportPlotAsPdf() {
            var history = new PlotHistory();
            var cmd = new ExportPlotAsPdfCommand(history);

            cmd.CommandID.ID.Should().Be(RPackageCommandId.icmdExportPlotAsPdf);
            cmd.Should().BeDisabled();

            history.ActivePlotIndex = 0;
            history.PlotCount = 1;
            cmd.Should().BeEnabled();
        }

        class PlotHistory : IPlotHistory {
            public int ActivePlotIndex { get; set; } = -1;
            public IPlotContentProvider PlotContentProvider { get; set; }

            public int PlotCount { get; set; }
#pragma warning disable 67
            public event EventHandler HistoryChanged;

            public void Dispose() {
            }

            public Task RefreshHistoryInfo() {
                return Task.CompletedTask;
            }
        }
    }
}