using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard
{
    abstract class WizardViewModelBase : ViewModelBase
    {
        #region Constructor

        public WizardViewModelBase()
        {
            this.BackCommand = new RelayCommand(this.Back);
            this.NextCommand = new RelayCommand(this.Next);
            this.FinishCommand = new RelayCommand(this.Finish);
        }

        #endregion

        #region Properties

        protected List<UserControl> WizardControls { get; set; } = new List<UserControl>();

        protected IEnumerable<WizardSubViewModelBase> Results
        {
            get { return this.WizardControls.Where(c => c.GetType() == typeof(WizardSubViewModelBase)).Select(c => (WizardSubViewModelBase) c.DataContext); }
        }

        protected int? ControlsIndex
        {
            get { return this.CurrentSubView != null && this.WizardControls.Contains(this.CurrentSubView) ? this.WizardControls.IndexOf(this.CurrentSubView) : default(int?); }
        }

        private UserControl currentSubView;
        public UserControl CurrentSubView
        {
            get { return this.currentSubView; }
            set
            {
                this.Set(ref this.currentSubView, value);

                this.HasPrevious = this.ControlsIndex > 0;
                this.HasNext = this.ControlsIndex < this.WizardControls.Count - 1;
                this.CanFinish = this.ControlsIndex == this.WizardControls.Count - 1;
            }
        }

        public ICommand BackCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand FinishCommand { get; set; }

        public ICommand CloseWindowCommand { get; set; }

        private bool hasPrevious;
        public virtual bool HasPrevious
        {
            get { return this.hasPrevious; }
            set { this.Set(ref this.hasPrevious, value); }
        }

        private bool hasNext;
        public virtual bool HasNext
        {
            get { return this.hasNext; }
            set { this.Set(ref this.hasNext, value); }
        }

        private bool canFinish;
        public virtual bool CanFinish
        {
            get { return this.canFinish; }
            set { this.Set(ref this.canFinish, value); }
        }

        #endregion

        #region Methods

        public void Start()
        {
            this.CurrentSubView = this.WizardControls.FirstOrDefault();
            ((WizardSubViewModelBase) this.CurrentSubView.DataContext).Loaded();
        }

        protected void Add(UserControl control)
        {
            this.WizardControls.Add(control);
            this.HasPrevious = this.ControlsIndex > 0;
            this.HasNext = this.ControlsIndex < this.WizardControls.Count - 1;
            this.CanFinish = this.ControlsIndex == this.WizardControls.Count - 1;
        }

        protected virtual void Back()
        {
            ((WizardSubViewModelBase) this.CurrentSubView.DataContext).Closed();

            if (this.HasPrevious)
                this.CurrentSubView = this.WizardControls[this.ControlsIndex.Value - 1];

            this.HasPrevious = this.ControlsIndex > 0;
            this.HasNext = this.ControlsIndex < this.WizardControls.Count - 1;
            this.CanFinish = this.ControlsIndex == this.WizardControls.Count - 1;

            this.RaisePropertyChanged("HasNext");
        }

        protected virtual void Next()
        {
            if (this.HasNext)
            {
                this.CurrentSubView = this.WizardControls[this.ControlsIndex.Value + 1];
                ((WizardSubViewModelBase) this.CurrentSubView.DataContext).Loaded();
            }

            this.HasPrevious = this.ControlsIndex > 0;
            this.HasNext = this.ControlsIndex < this.WizardControls.Count - 1;
            this.CanFinish = this.ControlsIndex == this.WizardControls.Count - 1;
        }

        protected virtual void Finish()
        {
            IEnumerable<WizardSubViewModelBase> results = this.Results;

            foreach (WizardSubViewModelBase result in results)
                result.Finish();

            this.CloseWindowCommand.Execute(null);
        }

        #endregion
    }
}
