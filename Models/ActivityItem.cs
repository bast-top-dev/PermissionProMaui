using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for activity items displayed on the home page
    /// </summary>
    public class ActivityItem : INotifyPropertyChanged
    {
        private string _icon = string.Empty;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _time = string.Empty;

        /// <summary>
        /// Gets or sets the icon for the activity
        /// </summary>
        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title of the activity
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the description of the activity
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the time when the activity occurred
        /// </summary>
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 