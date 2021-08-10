using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuggleTranslator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region BindingProperty
        private bool _locked;
        public bool Locked
        {
            get => _locked;
            set
            {
                _locked = value;
                RaisePropertyChanged(nameof(Locked));
            }
        }
        #endregion
    }
}
