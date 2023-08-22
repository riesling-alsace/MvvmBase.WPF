using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Riesling.Library.Mvvm {
	public class ModelBase : BindableBase {

		#region Setter

		protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
			lock (this) {
				if (EqualityComparer<T>.Default.Equals(storage, value)) {
					return false;
				}

				storage = value;
			}
			RaisePropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// プロパティの値をセットし、内容が変化していた場合は指定のコールバック関数を呼び出す。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="storage"></param>
		/// <param name="value"></param>
		/// <param name="onChanged">void callback()</param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected override bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null) {
			lock (this) {
				if (EqualityComparer<T>.Default.Equals(storage, value)) {
					return false;
				}

				storage = value;
				onChanged();
			}
			RaisePropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// プロパティの値をセットし、内容が変化していた場合は指定のコールバック関数を呼び出す。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="storage"></param>
		/// <param name="value"></param>
		/// <param name="onChanged">void callback(T newItems, T oldItems)</param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected override bool SetProperty<T>(ref T storage, T value, Action<T, T> onChanged, [CallerMemberName] string propertyName = null) {
			lock (this) {
				if (EqualityComparer<T>.Default.Equals(storage, value)) {
					return false;
				}

				var oldValue = storage;

				storage = value;
				onChanged(value, oldValue);
			}
			RaisePropertyChanged(propertyName);
			return true;
		}

		#endregion

	}
}
