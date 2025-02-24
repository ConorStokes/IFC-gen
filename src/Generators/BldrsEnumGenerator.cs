﻿using Bldrs.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Express;

namespace IFC4.Generators
{
    public static class BldrsEnumGenerator
    {
        public static string GenerateEnumString( EnumType data )
        {
            var typeIDGenerator = new BlrdrsTypeIDGenerator( data.Values.Select( name => $".{name}."), data.Values.Select( _ => true ) );

            var builder = new StringBuilder();

            typeIDGenerator.GenerateEnum(builder, data.SanitizedName(), 0, false);
            builder.AppendLine();
            typeIDGenerator.GenerateHashData(builder, data.SanitizedName(), null, 0, false);
            builder.AppendLine();
            builder.Append($@"
/* This is generated cold, don't alter */
import StepEnumParser from '../../step/parsing/step_enum_parser'

const parser = StepEnumParser.Instance

export function {data.SanitizedName()}DeserializeStep(
  input: Uint8Array,
  cursor: number,
  endCursor: number ): {data.SanitizedName()} | undefined {{
  return parser.extract< {data.SanitizedName()} >( {data.SanitizedName()}Search, input, cursor, endCursor )
}}
");

            return builder.ToString();
        }
    }
}
