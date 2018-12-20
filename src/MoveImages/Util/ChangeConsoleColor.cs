using System;
using System.Drawing;

namespace MoveImages.Util
{
    public class ChangeConsoleColor : IDisposable
    {
        private readonly ConsoleColor? _originalForegroundColor;
        private readonly ConsoleColor? _originalBackgroundColor;
        public ChangeConsoleColor(
            ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null
        )
        {
            if (foregroundColor != null)
            {
                _originalForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor.Value;
            }

            if (backgroundColor != null)
            {
                _originalBackgroundColor = Console.BackgroundColor;
                Console.BackgroundColor = backgroundColor.Value;
            }
        }

        #region IDisposable Support

        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_originalBackgroundColor.HasValue)
                        Console.BackgroundColor = _originalBackgroundColor.Value;
                    if (_originalForegroundColor.HasValue)
                        Console.ForegroundColor = _originalForegroundColor.Value;
                }

                _disposed = true;
            }
        }

        public void Dispose() => Dispose(true);

        #endregion

    }
}