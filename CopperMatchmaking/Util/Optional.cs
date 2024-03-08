using System;

namespace CopperMatchmaking.Util
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Optional<T>
    {
        /// <summary>
        /// Current state
        /// </summary>
        public bool Enabled = true;
        
        /// <summary>
        /// Current value
        /// </summary>
        public T Value;

        /// <summary>
        /// Creates a new optional class with a default value
        /// </summary>
        public Optional()
        {
            Value = default!;
        }
        
        /// <summary>
        /// Creates a new optional class with a specified value
        /// </summary>
        /// <param name="value"></param>
        public Optional(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Get the value of a optional class
        /// </summary>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static implicit operator T(Optional<T> optional) => optional.Value;
        
        /// <summary>
        /// Create a new optional value based off of a optional value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Optional<T>(T value) => new Optional<T> { Value = value };
        
        /// <summary>
        /// Get the state of the optional value
        /// </summary>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static implicit operator bool(Optional<T> optional) => optional.Enabled;

        // im too lazy to write xml comments for these
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        
        public static bool operator ==(Optional<T> lhs, Optional<T> rhs) {
            if (lhs.Value is null)
            {
                return rhs.Value is null;
            }
            return lhs.Value.Equals(rhs.Value);
        }

        public static bool operator !=(Optional<T> leftSide, Optional<T> rightSide) {
            return !(leftSide == rightSide);
        }
        
        public override bool Equals(object obj) {
            // return base.Equals(obj);
            return Value.Equals(obj);
        }
        
        public override int GetHashCode() {
            // return base.GetHashCode();
            return Value.GetHashCode();
        }

        public override string ToString() => Value.ToString();
    }
}