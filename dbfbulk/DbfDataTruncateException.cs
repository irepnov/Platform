using System;
using System.Runtime.Serialization;


namespace GGPlatform.DBFBulk
{

    public class DbfDataTruncateException: Exception
  { 
    
    public DbfDataTruncateException(string smessage): base(smessage)
    { 
    }

    public DbfDataTruncateException(string smessage, Exception innerException)
      : base(smessage, innerException)
    { 
    }

    public DbfDataTruncateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
    
  }
}
