using GPACARICOMAPI.Models;
using GPACARICOMAPI.Repositories.Interface;
using GPACARICOMAPI.Services.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;

namespace GPACARICOMAPI.Repositories;

public class WorkAndTravelApplicationProgressRepository
    
{
    private readonly IConnectionFactory _connectionFactory;

    public WorkAndTravelApplicationProgressRepository(
        IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    
}