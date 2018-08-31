﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legendary_Rune_Maker.Data.Rune_providers
{
    internal abstract class RuneProvider
    {
        public abstract string Name { get; }

        private IDictionary<int, Position[]> PossibleRolesCache = new Dictionary<int, Position[]>();
        private IDictionary<(int, Position), RunePage> RunePageCache = new Dictionary<(int, Position), RunePage>();

        public async Task<Position[]> GetPossibleRoles(int championId)
            => PossibleRolesCache.TryGetValue(championId, out var r) ? r :
               PossibleRolesCache[championId] = await GetPossibleRolesInner(championId);

        public async Task<RunePage> GetRunePage(int championId, Position position)
            => RunePageCache.TryGetValue((championId, position), out var r) ? r :
               RunePageCache[(championId, position)] = await GetRunePageInner(championId, position);

        protected abstract Task<Position[]> GetPossibleRolesInner(int championId);
        protected abstract Task<RunePage> GetRunePageInner(int championId, Position position);
    }
}