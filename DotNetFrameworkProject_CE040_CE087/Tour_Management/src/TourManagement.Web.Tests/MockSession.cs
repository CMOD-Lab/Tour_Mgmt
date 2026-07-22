using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TourManagement.Web.Tests
{
    /// <summary>
    /// A simple in-memory ISession implementation for unit testing.
    /// </summary>
    public class MockSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _store.Keys;

        public void Clear() => _store.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value) => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value!);

        // Convenience helpers matching ISession extension methods
        public void SetString(string key, string value)
            => Set(key, Encoding.UTF8.GetBytes(value));

        public string? GetString(string key)
            => TryGetValue(key, out var bytes) ? Encoding.UTF8.GetString(bytes) : null;

        public void SetInt32(string key, int value)
            => Set(key, BitConverter.GetBytes(value));

        public int? GetInt32(string key)
        {
            if (!TryGetValue(key, out var bytes)) return null;
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
