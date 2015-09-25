﻿/** 
 * Copyright (C) 2015 smndtrl
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libtextsecure.push
{
    [JsonObject(MemberSerialization.OptIn)]
    class AccountAttributes
    {
        [JsonProperty]
        private String signalingKey;

        [JsonProperty]
        private bool supportsSms;

        [JsonProperty]
        private uint registrationId;

        public AccountAttributes(String signalingKey, bool supportsSms, uint registrationId)
        {
            this.signalingKey = signalingKey;
            this.supportsSms = supportsSms;
            this.registrationId = registrationId;
        }

        public AccountAttributes() { }

        public String getSignalingKey()
        {
            return signalingKey;
        }

        public bool isSupportsSms()
        {
            return supportsSms;
        }

        public uint getRegistrationId()
        {
            return registrationId;
        }
    }
}