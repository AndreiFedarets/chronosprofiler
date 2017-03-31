using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	[Serializable]
	public sealed class FunctionInfo : UnitBase
	{
		public FunctionInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName, ulong classManagedId, uint hits, uint totalTime)
			: base(id, managedId, beginLifetime, endLifetime, fullName)
		{
			ClassManangedId = classManagedId;
			Hits = hits;
			TotalTime = totalTime;
		}

		public FunctionInfo()
        {
            ClassManangedId = 0;
            Hits = 0;
            TotalTime = 0;
		}
		public ulong ClassManangedId { get; set; }

		public uint Hits { get; set; }

		public uint TotalTime { get; set; }

	    public ClassInfo Class
	    {
	        get
            {
                //Temporary fix for NullReferenceExcption
                if (ClassManangedId == 0)
                {
                    return ClassInfo.Unknown;
                }
                List<ClassInfo> classes = ProcessShadow.Classes.Snapshot();
                ClassInfo classInfo = classes.FirstOrDefault(BelongsTo);
                return classInfo;
	        }
	    }

        public bool BelongsTo(ClassInfo classInfo)
        {
            return classInfo.ManagedId == ClassManangedId && BeginLifetime >= classInfo.BeginLifetime &&
                   (EndLifetime <= classInfo.EndLifetime || EndLifetime == 0);
        }
	}
}
