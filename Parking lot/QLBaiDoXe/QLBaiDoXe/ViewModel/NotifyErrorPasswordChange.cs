using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace QLBaiDoXe
{
    public class NotifyErrorPasswordChange : ObservableObject, INotifyDataErrorInfo
    {
        //Khởi tạo loại textbox tương ứng trong đây:

        private string currentPwd;
        public string CurrentPwd
        {
            get => currentPwd;
            set => SetProperty(ref currentPwd, value);
        }

        private string newPwd;
        public string NewPwd
        {
            get => newPwd;
            set => SetProperty(ref newPwd, value);
        }

        private string reNewPwd;
        public string ReNewPwd
        {
            get => reNewPwd;
            set => SetProperty(ref reNewPwd, value);
        }
        public RelayCommand SubmitCommand { get; }

        public NotifyErrorPasswordChange()
        {
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
            //Doing this will cause the errors to show immediately
            Validate(nameof(CurrentPwd));
            Validate(nameof(NewPwd));
            Validate(nameof(ReNewPwd));
        }

        private bool CanSubmit()
        {
            //Link the CanExecute state of the command to the visible errors on the screen. 
            //You can also separate the command from the validation errors and simply change this to match the one in SimpleViewModel.CanSubmit
            return !HasErrors;
        }

        private void OnSubmit()
        {
            Debug.WriteLine("Form Submitted");
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            Validate(e.PropertyName);
        }

        private void Validate(string changedPropertyName)
        {
            //Do whatever validation is needed here
            //You can do validation accross multiple properties as well.
            switch (changedPropertyName)
            {
                case nameof(CurrentPwd):
                    if (string.IsNullOrWhiteSpace(CurrentPwd))
                    {
                        _ValidationErrorsByProperty[nameof(CurrentPwd)] = new List<object> { " Trường dữ liệu bắt buộc." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CurrentPwd)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(CurrentPwd)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CurrentPwd)));
                    }
                    break;

                case nameof(NewPwd):
                    if (string.IsNullOrWhiteSpace(NewPwd))
                    {
                        _ValidationErrorsByProperty[nameof(NewPwd)] = new List<object> { " Trường dữ liệu bắt buộc." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewPwd)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(NewPwd)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewPwd)));
                    }
                    break;

                case nameof(ReNewPwd):
                    if (string.IsNullOrWhiteSpace(ReNewPwd))
                    {
                        _ValidationErrorsByProperty[nameof(ReNewPwd)] = new List<object> { " Trường dữ liệu bắt buộc." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ReNewPwd)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ReNewPwd)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ReNewPwd)));
                    }
                    break;

            }
            SubmitCommand.NotifyCanExecuteChanged();
        }


        public IEnumerable GetErrors(string propertyName)
        {
            if (_ValidationErrorsByProperty.TryGetValue(propertyName, out List<object> errors))
            {
                return errors;
            }
            return Array.Empty<object>();
        }

        private readonly Dictionary<string, List<object>> _ValidationErrorsByProperty =
            new Dictionary<string, List<object>>();

        public bool HasErrors => _ValidationErrorsByProperty.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
