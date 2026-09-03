// Copyright (c) CypherCore <http://github.com/CypherCore> All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.

using Framework.Constants;
using Game.Entities;

namespace Game.Networking.Adapters.V1_14_0
{
    /// <summary>
    /// Translates the player-inventory slot numbers a 1.14 client sends into the
    /// slot space the core uses internally.
    ///
    /// The two disagree from the bag slots onwards, because the core reserves room
    /// for reagent bags and a 24-slot backpack where 1.14 does not:
    ///
    ///     region        internal      1.14 client
    ///     equipment       0..18          0..18      (identical)
    ///     bag slots      30..33         19..22
    ///     backpack       35..58         23..46
    ///     bank items     59..86         47..74
    ///     bank bags      87..93         75..81
    ///     buyback        94..105        82..93
    ///     keyring       106..137        94..125
    ///
    /// UpdateFieldsMapper1140 already applies this mapping on the way OUT, so the
    /// client sees its own numbering. Nothing applied the inverse on the way back
    /// IN, so a request naming backpack slot 23 was looked up at internal slot 23 --
    /// which is nothing at all. Equipping, swapping, splitting and destroying items
    /// from the bag therefore did nothing and reported no error: the handlers found
    /// no item at the position and returned silently.
    /// </summary>
    public static class InventorySlots1140
    {
        /// <summary>Maps a client-side PLAYER slot number to the internal one.</summary>
        public static byte ToInternal(byte clientSlot)
        {
            if (clientSlot == ItemSlot.Null)
                return clientSlot;                                  // "no container"
            if (clientSlot < 19)
                return clientSlot;                                  // equipment, identical
            if (clientSlot < 23)
                return (byte)(InventorySlots.BagStart + (clientSlot - 19));
            if (clientSlot < 47)
                return (byte)(InventorySlots.ItemStart + (clientSlot - 23));
            if (clientSlot < 75)
                return (byte)(InventorySlots.BankItemStart + (clientSlot - 47));
            if (clientSlot < 82)
                return (byte)(InventorySlots.BankBagStart + (clientSlot - 75));
            if (clientSlot < 94)
                return (byte)(InventorySlots.BuyBackStart + (clientSlot - 82));
            if (clientSlot < 126)
                return (byte)(InventorySlots.KeyringStart + (clientSlot - 94));

            return clientSlot;
        }

        /// <summary>
        /// Builds an internal ItemPos from a client (slot, container) pair.
        /// When the item sits inside a real bag the slot is an index WITHIN that bag
        /// and must be left alone -- only the container itself is a player slot.
        /// </summary>
        public static ItemPos ToItemPos(byte clientSlot, byte clientContainer)
        {
            if (clientContainer == ItemSlot.Null)
                return new ItemPos(ToInternal(clientSlot), ItemSlot.Null);

            return new ItemPos(clientSlot, ToInternal(clientContainer));
        }
    }
}
