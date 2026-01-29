using System;
using System.Collections;


namespace Doctor_Booking.Application.ViewModels
{
	public class ResponseViewModel<T>
	{
		public T? Data { get; set; }

		public bool IsSucsess { get; set; }

		public int Status { get; set; }

		public string Message { get; set; }

		public List<object> Errors { get; set; } = new();

		public static ResponseViewModel<T> SuccessResponse(
			T data = default,
			int status = 200,
			string message = "Request completed successfully")
		{
			// If the data type is a collection and it's null, initialize it to an empty one
			if (data == null && typeof(IEnumerable).IsAssignableFrom(typeof(T)) && typeof(T) != typeof(string))
			{
				var elementType = typeof(T).IsGenericType
					? typeof(T).GetGenericArguments()[0]
					: typeof(object);
				data = (T)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;
			}

			return new ResponseViewModel<T>
			{
				Data = data,
				IsSucsess = true,
				Status = status,
				Message = message,
				Errors = new List<object>()
			};
		}

		public static ResponseViewModel<T> FailureResponse(
			string message,
			int status = 400,
			T? data = default,
			List<object>? errors = null)
		{
			if (data == null)
			{
				var enumerableInterface = typeof(T)
					.GetInterfaces()
					.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

				if (enumerableInterface != null)
				{
					var elementType = enumerableInterface.GetGenericArguments()[0];
					var listType = typeof(List<>).MakeGenericType(elementType);
					data = (T)Activator.CreateInstance(listType)!;
				}
			}

			return new ResponseViewModel<T>
			{
				Data = data,
				IsSucsess = false,
				Status = status,
				Message = message,
				Errors = errors ?? new List<object>()
			};
		}
	}
}
