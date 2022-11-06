﻿using Global.Data;
using Global.Data.EffectSystem;
using HippieFall.Collectables;

namespace HippieFall.Tunnels.Effects
{
    public class ChangeCollectableWeight : Effect
    {
        private CollectableConfig _config;
        
        private bool _isValueCoefficient;
        public ChangeCollectableWeight(CollectableConfig collectableConfig, bool isValueCoefficient)
        {
            Target = EffectsTarget.Collectable;
            _config = collectableConfig;
            _isValueCoefficient = isValueCoefficient;
        }

        public override Config Apply(Config config)
        {
            if (config is CollectableConfig collectableConfig)
            {
                if (_config is CollectableCoinConfig && collectableConfig is CollectableCoinConfig coinConfig)
                    ApplyEffect(coinConfig);
                if (_config is CollectableGemcoinConfig && collectableConfig is CollectableGemcoinConfig gemcoinConfig)
                    ApplyEffect(gemcoinConfig);
                if (_config is CollectableChestConfig && collectableConfig is CollectableChestConfig chestConfig)
                    ApplyEffect(chestConfig);
            }
            return config;
        }

        private void ApplyEffect(CollectableCoinConfig coinConfig)
        {
            if (_isValueCoefficient)
                coinConfig.SpawnWeight *= _config.SpawnWeight;
            else
                coinConfig.SpawnWeight = _config.SpawnWeight;
        }

        private void ApplyEffect(CollectableGemcoinConfig gemcoinConfig)
        {
            
        }

        private void ApplyEffect(CollectableChestConfig chestConfig)
        {
            
        }
    }
}