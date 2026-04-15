using System.Windows;

namespace Riesling.Mvvms.WPF;

public class ViewModelBase : ModelBase {

	#region Raise Methods

    protected override void RaisePropertyChanged(params string[] propertyNames) {
		if (Application.Current.Dispatcher.CheckAccess()) {
			base.RaisePropertyChanged(propertyNames);
		} else {
            Application.Current.Dispatcher.BeginInvoke(() => {
				lock (this) {
					base.RaisePropertyChanged(propertyNames);
				}
			});
		}
	}

	#endregion

}

public abstract class ViewModelBase<TModelBase> : ViewModelBase
    where TModelBase : ModelBase, new() {

    #region Properties

    public TModelBase Model {
        get;
        init {
            field = value;
            Model.PropertiesChanged += Model_PropertiesChanged;
        }
    }

    #endregion

    #region Constructors

    protected ViewModelBase(TModelBase model) {
        Model = model;
    }

    ~ViewModelBase() {
        Model.PropertiesChanged -= Model_PropertiesChanged;
    }

    #endregion

    #region Methods

    protected abstract void Model_PropertiesChanged(object? sender, PropertiesChangedEventArgs e);

    #endregion

}
