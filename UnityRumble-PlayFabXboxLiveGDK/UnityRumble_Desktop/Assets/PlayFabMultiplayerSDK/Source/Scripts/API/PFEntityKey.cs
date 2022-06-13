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
    /// PFEntityKey data model. Combined entity type and ID structure which uniquely identifies a single entity.
    /// </summary>
    /// <remarks>
    /// For more information about entities, see https://docs.microsoft.com/en-us/gaming/playfab/features/data/entities/.
    /// </remarks>
    public class PFEntityKey
    {
#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Initializes a new instance of the <see cref="PFEntityKey" /> class.
        /// Pass in a PlayFabAuthenticationContext <paramref name="authContext" /> returned by a PlayFab login method.
        /// </summary>
        public PFEntityKey(PlayFab.PlayFabAuthenticationContext authContext)
        {
            this.EntityKey = new InteropWrapper.PFEntityKey(authContext.EntityId, authContext.EntityType);
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="PFEntityKey" /> class.
        /// </summary>
        public PFEntityKey(string id, string type)
        {
            this.EntityKey = new InteropWrapper.PFEntityKey(id, type);
        }

        internal PFEntityKey(InteropWrapper.PFEntityKey newEntityKey)
        {
            this.EntityKey = newEntityKey;
        }

        /// <summary>
        /// Unique ID of the entity.
        /// </summary>
        public string Id
        {
            get
            {
                return this.EntityKey.Id;
            }

            set
            {
                this.EntityKey.Id = value;
            }
        }

        /// <summary>
        /// Entity type. See https://docs.microsoft.com/gaming/playfab/features/data/entities/available-built-in-entity-types.
        /// </summary>
        /// <remarks>
        /// Player entities are typically the <c>title_player_account</c> type. For more information, see See
        /// https://docs.microsoft.com/gaming/playfab/features/data/entities/available-built-in-entity-types.
        /// </remarks>
        public string Type
        {
            get
            {
                return this.EntityKey.Type;
            }

            set
            {
                this.EntityKey.Type = value;
            }
        }

        internal InteropWrapper.PFEntityKey EntityKey { get; set; }
    }
}
