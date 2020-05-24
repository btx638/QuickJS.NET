using System;
using System.Linq;

namespace QuickJS
{
	/// <summary>
	/// Represents errors taht occurs during JS execution.
	/// </summary>
	public class QuickJSException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="QuickJSException"/>.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public QuickJSException(string message)
			: base(message)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QuickJSException"/>.
		/// </summary>
		/// <param name="errorInfo">The error information.</param>
		public QuickJSException(in ErrorInfo errorInfo)
			: base(errorInfo.Message)
		{
			string stack = errorInfo.Stack;
			this.Name = errorInfo.Name;
			this.Stack = stack;

			if (!string.IsNullOrEmpty(stack))
			{
				int startPos;
				int endPos = stack.IndexOf('\n');
				if (endPos == -1)
					endPos = stack.Length - 1;
				endPos = stack.LastIndexOf(')', endPos);

				string source;
				if (endPos != -1)
				{
					startPos = stack.LastIndexOf('(', endPos) + 1;
					source = stack.Substring(startPos, endPos - startPos);
				}
				else // special case for SyntaxError.
				{
					endPos = stack.IndexOf('\n');
					if (endPos == -1)
						endPos = stack.Length;
					startPos = stack.IndexOf(" at ", StringComparison.InvariantCulture) + 4;
					source = stack.Substring(startPos, endPos - startPos);
				}
				endPos = source.LastIndexOf(':');
				if (endPos != -1 && source.Skip(endPos + 1).All(char.IsDigit))
				{
					this.FileName = source.Remove(endPos);
					if (endPos + 1 < source.Length)
					{
						this.LineNumber = int.Parse(source.Substring(endPos + 1));
					}
				}
				else
				{
					this.FileName = source;
				}
			}
		}

		/// <summary>
		/// Gets the error name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Get the JavaScript stack trace.
		/// </summary>
		public string Stack { get; }

		/// <summary>
		/// Gets the path to the file that raised a error.
		/// </summary>
		public string FileName { get; }

		/// <summary>
		/// Gets the line number in the file that raised a error.
		/// </summary>
		public int LineNumber { get; }

		/// <inheritdoc/>
		public override string ToString()
		{
			if (this.Name is null)
				return base.ToString();
			return string.Format("{0}{1}--- JS exception ---{1}{2}: {3}{1}{4}", base.ToString(), Environment.NewLine, Name, Message, Stack);
		}
	}
}
