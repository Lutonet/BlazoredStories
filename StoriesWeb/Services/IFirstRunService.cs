﻿using StoriesWeb.Models.Dto;
using StoriesWeb.Models.Helpers;
using ToolsLibrary.Models;

namespace StoriesWeb.Services
{
  public interface IFirstRunService
  {
    Task<ApiResponse<string>> CreateAdministrator(AdministratorDto administrator);

    Task<ApiResponse<string>> CreateAgeRestrictions();

    Task<ApiResponse<string>> CreateApplicationSettings(ApplicationSettingsDto settings);

    Task<ApiResponse<string>> CreateCategories();

    Task<ApiResponse<string>> CreateCategoryGroups();

    Task<ApiResponse<string>> CreateCountries();

    Task<ApiResponse<string>> CreateDefaultRoles();

    Task<ApiResponse<string>> CreateLanguagesTable();
  }
}