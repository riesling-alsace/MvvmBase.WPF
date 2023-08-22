using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Riesling.Library.Mvvm {
	public class ViewModelBase : ModelBase {

		private static Dispatcher UIDispatcher => Application.Current.Dispatcher;

		#region Events

		protected override void RaisePropertiesChanged(IEnumerable<string> propertyNames) {
			if (UIDispatcher.CheckAccess()) {
				base.RaisePropertiesChanged(propertyNames);
			} else {
				UIDispatcher.BeginInvoke(() => {
					lock (this) {
						base.RaisePropertiesChanged(propertyNames);
					}
				});
			}
		}

		#endregion

	}

	public abstract class ViewModelBase<TModelBase> : ViewModelBase
		where TModelBase : ModelBase {

		#region Instances

		protected TModelBase? _Model;

		#endregion

		#region Properties

		public TModelBase? Model {
			get => _Model;
			set => SetProperty(ref _Model, value, Model_Changed);
		}

		protected virtual void Model_Changed(TModelBase? newModel, TModelBase? oldModel) {
			if (oldModel != null) {
				oldModel.PropertyChanged -= Model_PropertyChanged;
			}
			if (newModel != null) {
				newModel.PropertyChanged += Model_PropertyChanged;
			}
		}

		#endregion

		#region Constructor

		public ViewModelBase(TModelBase? model = null) {
			Model = model;
		}

		protected abstract void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e);

		#endregion

	}
}
