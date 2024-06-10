using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using RenameFile.Constants;
using RenameFile.DataSources;
using RenameFile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RenameFile.ViewModels
{
    public class RenameFileViewModel : BindableBase
    {
        private string _directoryPath = string.Empty;

        public string DirectoryPath
        {
            get => this._directoryPath;
            set => this.SetProperty(ref this._directoryPath, value);
        }

        private string _fileNamePattern = string.Empty;

        public string FileNamePattern
        {
            get => this._fileNamePattern;
            set => this.SetProperty(ref this._fileNamePattern, value);
        }

        private string _extensions = string.Empty;

        public string Extensions
        {
            get => this._extensions;
            set => this.SetProperty(ref this._extensions, value);
        }

        private string _sequenceFormat = string.Empty;

        public string SequenceFormat
        {
            get => this._sequenceFormat;
            set => this.SetProperty(ref this._sequenceFormat, value);
        }

        private DateTime _datetimeValue = DateTime.Now;

        public DateTime DatetimeValue
        {
            get => this._datetimeValue;
            set => this.SetProperty(ref this._datetimeValue, value);
        }

        private string _datetimeFormat = string.Empty;

        public string DatetimeFormat
        {
            get => this._datetimeFormat;
            set => this.SetProperty(ref this._datetimeFormat, value);
        }

        private string _example = string.Empty;

        public string Example
        {
            get => this._example;
            set => this.SetProperty(ref this._example, value);
        }

        public DelegateCommand RenameCommand { get; set; }

        public RenameFileViewModel()
        {
            //this.SaveConfig();
            this.LoadConfig();

            // ファイル名をリネーム
            this.RenameCommand = new DelegateCommand(
                () =>
                {
                    var ds = this.CreateRenameFileDataSource();
                    var model = new RenameFileModel(ds);
                    model.Rename();
                });
        }

        private RenameFileDataSource CreateRenameFileDataSource()
        {
            return new()
            {
                RootDirectoryPath = this.DirectoryPath,
                FileNamePattern = this.FileNamePattern,
                ContainsParentDirectoryNameTemplate = this.FileNamePattern.Contains(FileNameTemplate.PARENT_DIRECTORY_NAME_TEMPLATE),
                ContainsSequenceTemplate = this.FileNamePattern.Contains(FileNameTemplate.SEQUENCE_TEMPLATE),
                ShouldInitializeSequence = true,
                InitialSequence = 1,
                SequenceFormat = this.SequenceFormat,
                ContainsFimeNameDatetimeTemplate = this.FileNamePattern.Contains(FileNameTemplate.DATETIME_TEMPLATE),
                FimeNameDatetime = this.DatetimeValue,
                FimeNameDatetimeFormat = this.DatetimeFormat,
                ContainsUniqueIdentifierTemplate = false,
                UniqueIdentifierFormat = string.Empty,
                TargetFileExtensions = [.. this.Extensions.Split(',')],
            };
        }

        private void LoadConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            this.DirectoryPath = config.AppSettings.Settings[nameof(this.DirectoryPath)].Value;
            this.FileNamePattern = config.AppSettings.Settings[nameof(this.FileNamePattern)].Value;
            this.SequenceFormat = config.AppSettings.Settings[nameof(this.SequenceFormat)].Value;
            this.DatetimeFormat = config.AppSettings.Settings[nameof(this.DatetimeFormat)].Value;
            this.Extensions = config.AppSettings.Settings[nameof(this.Extensions)].Value;
        }

        private void SaveConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[nameof(this.DirectoryPath)].Value = this.DirectoryPath;
            config.AppSettings.Settings[nameof(this.FileNamePattern)].Value = this.FileNamePattern;
            config.AppSettings.Settings[nameof(this.SequenceFormat)].Value = this.SequenceFormat;
            config.AppSettings.Settings[nameof(this.DatetimeFormat)].Value = this.DatetimeFormat;
            config.AppSettings.Settings[nameof(this.Extensions)].Value = this.Extensions;

            config.Save();
        }
    }
}
