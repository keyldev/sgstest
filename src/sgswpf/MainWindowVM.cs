using sgswpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace sgswpf
{
    internal class MainWindowVM : ViewModelBase
    {
        private ObservableCollection<City> _citiesList;

        public ObservableCollection<City> CitiesList
        {
            get { return _citiesList; }
            set { _citiesList = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<Workshop> _workshopList;

        public ObservableCollection<Workshop> WorkshopsList
        {
            get { return _workshopList; }
            set { _workshopList = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<Employee> _workersList;

        public ObservableCollection<Employee> WorkersList
        {
            get { return _workersList; }
            set { _workersList = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<Brigade> _brigadeList;

        public ObservableCollection<Brigade> BrigadesList
        {
            get { return _brigadeList; }
            set { _brigadeList = value; NotifyPropertyChanged(); }
        }
        private string _shiftText;

        public string ShiftText
        {
            get { return _shiftText; }
            set
            {
                _shiftText = value; NotifyPropertyChanged();
                if (_selectedEmployee != null)
                {
                    _selectedEmployee.Shift = Convert.ToInt32(value);
                }
                else
                {
                    _shiftText = null;
                }
            }
        }

        private City _selectedCity;

        public City SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;

                NotifyPropertyChanged();

                FilteredWorkshops.Filter = item =>
                {
                    var workshop = item as Workshop;

                    return workshop.CityId == _selectedCity.Id;
                };
            }
        }
        private Workshop _selectedWorkshop;

        public Workshop SelectedWorkshop
        {
            get { return _selectedWorkshop; }
            set
            {
                _selectedWorkshop = value; NotifyPropertyChanged();
                FilteredBrigades.Filter = item =>
                {
                    var brigade = item as Brigade;
                    return brigade != null && _selectedWorkshop != null && _selectedWorkshop.Id == brigade.WorkshopId;
                };

            }
        }
        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value; NotifyPropertyChanged();
                if (_selectedEmployee != null)
                    ShiftText = _selectedEmployee.Shift.ToString();

            }
        }
        private Brigade _selectedBrigade;

        public Brigade SelectedBrigade
        {
            get { return _selectedBrigade; }
            set
            {
                _selectedBrigade = value; NotifyPropertyChanged();
                FilteredWorkers.Filter = item =>
                {
                    var employee = item as Employee;

                    return employee != null && _selectedBrigade != null && employee.BrigadeId == _selectedBrigade.Id;
                };
            }
        }

        public ICollectionView FilteredWorkshops { get; set; }
        public ICollectionView FilteredBrigades { get; private set; }
        public ICollectionView FilteredWorkers { get; private set; }

        public MainWindowVM()
        {
            LoadData();
        }
        private void LoadData()
        {
            CitiesList = new ObservableCollection<City> {
                new City { Id = 1, CityName = "Москва" },
                new City { Id = 2, CityName = "Берлин" },
                new City { Id = 3, CityName = "Санкт-Петербург" },
                new City{ Id = 4, CityName = "Тбилиси" },
                new City{ Id = 5, CityName = "Лондон" },
            };
            WorkshopsList = new ObservableCollection<Workshop>()
            {
                new Workshop
                {
                    Id = 1,
                    CityId = 1,
                    WorkshopName = "Цех 1",

                },new Workshop
                {
                    Id = 2,
                    CityId = 1,
                    WorkshopName = "Цех 2"
                },new Workshop
                {
                    Id = 3,
                    CityId = 2,
                    WorkshopName = "Цех 3"
                },new Workshop
                {
                    Id = 4,
                    CityId = 4,
                    WorkshopName = "Цех 4"
                },new Workshop
                {
                    Id = 5,
                    CityId = 5,
                    WorkshopName = "Цех 5"
                },
            };
            WorkersList = new ObservableCollection<Employee>()
            {
                new Employee
                {
                    Id = 1,
                    WorkshopId = 1,
                    EmployeeName = "Имя 1",
                    Shift = 1,
                    BrigadeId = 1,
                },new Employee
                {
                    Id = 2,
                    WorkshopId = 1,
                    EmployeeName = "Имя 2",
                    Shift = 1,
                    BrigadeId = 2
                },new Employee
                {
                    Id = 3,
                    WorkshopId = 3,
                    EmployeeName = "Имя 3",
                    Shift = 1,
                    BrigadeId = 3
                },new Employee
                {
                    Id = 4,
                    WorkshopId = 4,
                    EmployeeName = "Имя 4",
                    Shift = 1,
                    BrigadeId = 1
                },new Employee
                {
                    Id = 5,
                    WorkshopId = 2,
                    EmployeeName = "Имя 5",
                    Shift = 1,
                    BrigadeId = 3
                },
            };
            BrigadesList = new ObservableCollection<Brigade>()
            {
                new Brigade()
                {
                    Id = 1,
                    WorkshopId = 1,
                    Name = "Бригада имени 1 (wid 1)",
                },
                new Brigade()
                {
                    Id = 2,
                    WorkshopId = 2,
                    Name = "Бригада имени 1 (wid 2)",
                },
                new Brigade()
                {
                    Id = 3,
                    WorkshopId = 1,
                    Name = "Бригада имени 2 (wid 1)",
                },
                new Brigade()
                {
                    Id = 4,
                    WorkshopId = 2,
                    Name = "Бригада имени 2 (wid 2)",
                },
                new Brigade()
                {
                    Id = 5,
                    WorkshopId = 1,
                    Name = "Бригада имени 3",
                },
            };

            FilteredWorkers = CollectionViewSource.GetDefaultView(WorkersList);
            FilteredWorkshops = CollectionViewSource.GetDefaultView(WorkshopsList);
            FilteredBrigades = CollectionViewSource.GetDefaultView(BrigadesList);

        }
    }
    class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
    }
    class Workshop
    {
        public int Id { get; set; }
        public string WorkshopName { get; set; }
        public int CityId { get; set; }
    }
    class Employee
    {
        public int Id { get; set; }
        public string? EmployeeName { get; set; }
        public int WorkshopId { get; set; } // nav prop?
        public int BrigadeId { get; set; }
        public int Shift { get; set; } // shift string?
    }
    class Brigade
    {
        public int Id { get; set; }
        public int WorkshopId { get; set; }
        public string Name { get; set; }

    }


}
