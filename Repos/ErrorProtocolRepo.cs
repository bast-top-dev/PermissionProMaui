using System.Collections.Generic;
using PermissionProMaui.Models;
using SQLite;

namespace PermissionProMaui.Repos;

/// <summary>
/// Repository for managing error protocols in the database.
/// Note: This repository uses SettingsTable in the database for storage.
/// </summary>
public class ErrorProtocolRepo
{
    private readonly string _databasePath;

    public ErrorProtocolRepo(string databasePath)
    {
        _databasePath = databasePath;
    }

    /// <summary>
    /// Creates a new error protocol entry in the database.
    /// </summary>
    /// <param name="errorProtocol">The error protocol to create.</param>
    public void CreateErrorProtocol(SettingsTable errorProtocol)
    {
        using var databaseConnection = new SQLiteConnection(_databasePath);
        databaseConnection.Insert(errorProtocol);
    }

    /// <summary>
    /// Reads all error protocols from the database.
    /// </summary>
    /// <returns>List of all error protocols.</returns>
    public List<SettingsTable> ReadAllErrorProtocols()
    {
        using var databaseConnection = new SQLiteConnection(_databasePath);
        return databaseConnection.Table<SettingsTable>().ToList();
    }

    /// <summary>
    /// Updates an existing error protocol in the database.
    /// </summary>
    /// <param name="errorProtocol">The error protocol to update.</param>
    public void UpdateErrorProtocol(SettingsTable errorProtocol)
    {
        using var databaseConnection = new SQLiteConnection(_databasePath);
        databaseConnection.Update(errorProtocol);
    }

    /// <summary>
    /// Deletes a single error protocol from the database.
    /// </summary>
    /// <param name="errorProtocol">The error protocol to delete.</param>
    public void DeleteErrorProtocol(SettingsTable errorProtocol)
    {
        using var databaseConnection = new SQLiteConnection(_databasePath);
        databaseConnection.Delete(errorProtocol);
    }

    /// <summary>
    /// Deletes multiple error protocols from the database.
    /// </summary>
    /// <param name="errorProtocol">List of error protocols to delete.</param>
    public void DeleteErrorProtocol(List<SettingsTable> errorProtocol)
    {
        if (errorProtocol.Count == 0) return;

        using var databaseConnection = new SQLiteConnection(_databasePath);

        foreach (SettingsTable error in errorProtocol)
        {
            databaseConnection.Delete(error);
        }
    }
} 