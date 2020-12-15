using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Contact.ViewModel
{
    public class MainViewModel : ViewModelBase, IDataErrorInfo
    {

        private Dictionary<string, bool> validProperties;
        private bool allPropertiesValid = false;
        public MainViewModel()
        {
            ListContacts = Model.XML.GetContacts();
            validProperties = new Dictionary<string, bool>();
            validProperties.Add("Name", false);
            validProperties.Add("Phone", false);
        }

        private void UpdateContacts()
        {
            Model.XML.SaveContact(ListContacts);
            ListContacts = Model.XML.GetContacts();
        }

        /// <summary>
        /// For list contacts
        /// </summary>
        public ObservableCollection<Model.Contact> ListContacts
        {
            get
            {
                return listContacts;
            }
            set
            {
                listContacts = value;
                RaisePropertyChanged(() => ListContacts);
            }
        }
        private ObservableCollection<Model.Contact> listContacts;

        /// <summary>
        /// Selected item in listbox
        /// </summary>       
        private Model.Contact SelectedContact;


        /// <summary>
        /// Click create new contact
        /// </summary>
        public RelayCommand CreateContact
        {
            get
            {
                return createContact ?? (createContact = new RelayCommand(() =>
                {
                    SelectedContact = new Model.Contact();
                    EditContactVisibility = Visibility.Visible;
                }));
            }
        }
        private RelayCommand createContact;


        public string Error
        {
            get { return (SelectedContact as IDataErrorInfo).Error; }
        }

        public string this[string propertyName]
        {
            get
            {
                string error = (SelectedContact as IDataErrorInfo)[propertyName];
                validProperties[propertyName] = String.IsNullOrEmpty(error) ? true : false;
                ValidateProperties();
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        public bool AllPropertiesValid
        {
            get { return allPropertiesValid; }
            set
            {
                if (allPropertiesValid != value)
                {
                    allPropertiesValid = value;
                    RaisePropertyChanged(() => AllPropertiesValid);
                }
            }
        }


        private void ValidateProperties()
        {
            foreach (bool isValid in validProperties.Values)
            {
                if (!isValid)
                {
                    AllPropertiesValid = false;
                    return;
                }
            }
            AllPropertiesValid = true;
        }

        /// <summary>
        /// Visibility edit grid
        /// </summary>
        public Visibility EditContactVisibility
        {
            get
            {
                return editContactVisibility;
            }
            set
            {
                editContactVisibility = value;
                RaisePropertyChanged(() => EditContactVisibility);
            }
        }
        private Visibility editContactVisibility = Visibility.Hidden;


        /// <summary>
        /// Click create new contact
        /// </summary>
        public RelayCommand ClosingView
        {
            get
            {
                return closingView ?? (closingView = new RelayCommand(() =>
                {
                    Model.XML.SaveContact(ListContacts);
                }));
            }
        }
        private RelayCommand closingView;


        /// <summary>
        /// Click edit contact
        /// </summary>
        public RelayCommand<object> EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new RelayCommand<object>((x)=>
                {
                    try
                    {
                        int id = int.Parse(x?.ToString());
                        SelectedContact = ListContacts.FirstOrDefault(i => i.ID == id);
                        RaisePropertyChanged(() => Name);
                        RaisePropertyChanged(() => Phone);
                        EditContactVisibility = Visibility.Visible;
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }));
            }
        }
        private RelayCommand<object> editCommand;


        /// <summary>
        /// Click edit contact
        /// </summary>
        public RelayCommand<object> DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand<object>((x) =>
                {
                    try
                    {
                        if (MessageBox.Show("Вы действительно хотите удалить контакт?", "Предуприждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            int id = int.Parse(x?.ToString());
                            ListContacts.Remove(ListContacts.FirstOrDefault(i => i.ID == id));
                            UpdateContacts();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }));
            }
        }
        private RelayCommand<object> deleteCommand;


        /// <summary>
        /// Textbox FIO edit view 
        /// </summary>
        public string Name
        {
            get
            {
                return SelectedContact?.Name;
            }
            set
            {
                SelectedContact.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        private string name;

        /// <summary>
        /// Textbox Phone edit view 
        /// </summary>
        public string Phone
        {
            get
            {
                return SelectedContact?.Phone;
            }
            set
            {
                SelectedContact.Phone = value;
                RaisePropertyChanged(() => Phone);
            }
        }
        private string phone;


        /// <summary>
        /// Click save contact
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(() =>
                {
                    try
                    {
                        if (SelectedContact.ID == 0)
                        {
                            int ID = (ListContacts.Count != 0 ? ListContacts.Max(x => x.ID) + 1 : 1);
                            ListContacts.Add(new Model.Contact()
                            {
                                ID = ID,
                                Name = Name,
                                Phone = Phone
                            });
                        }
                        else
                        {
                            var item = ListContacts.FirstOrDefault(i => i.ID == SelectedContact.ID);
                            item.Name = Name;
                            item.Phone = Phone;
                        }

                        UpdateContacts();

                        EditContactVisibility = Visibility.Collapsed;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }));
            }
        }
        private RelayCommand saveCommand;
    }
}
