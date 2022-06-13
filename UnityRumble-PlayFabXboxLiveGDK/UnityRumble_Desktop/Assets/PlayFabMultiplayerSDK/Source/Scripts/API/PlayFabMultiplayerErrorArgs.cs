/*
 * PlayFab Unity SDK
 *
 * Copyright (c) Microsoft Corporation
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

namespace PlayFab.Multiplayer
{
    using System;

    /// <summary>
    /// An event argument class representing a PFMultiplayer error.
    /// </summary>
    public class PlayFabMultiplayerErrorArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayFabMultiplayerErrorArgs" /> class.
        /// </summary>
        /// <param name="code">
        /// The error code
        /// </param>
        /// <param name="message">
        /// The error message
        /// </param>
        internal PlayFabMultiplayerErrorArgs(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// Gets the error code indicating the result of the operation.
        /// </summary>
        public int Code
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a call-specific error message with debug information.
        /// This message is not localized as it is meant to be used for debugging only.
        /// </summary>
        public string Message
        {
            get;
            protected set;
        }
    }
}
