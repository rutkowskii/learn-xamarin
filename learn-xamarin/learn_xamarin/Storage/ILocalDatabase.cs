﻿using learn_xamarin.Model;
using learn_xamarin.Services;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace learn_xamarin.Storage
{
    public interface ILocalDatabase
    {
        void UpdateCategories(Category[] e);
        Category[] GetAllCategories();

        void Insert(UnsynchronizedItem unsynchronizedItem);
        Expenditure[] GetAllExpenditures();
        UnsynchronizedItem[] GetAllUnsynchronizedItems();
        void ClearUnsynchronizedItems();
        ConfigEntry[] GetAllConfigEntries();
    }
}