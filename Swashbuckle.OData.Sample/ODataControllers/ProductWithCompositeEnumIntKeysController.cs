﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SwashbuckleODataSample.Models;
using System;
using Microsoft.AspNet.OData;

namespace SwashbuckleODataSample.ODataControllers
{
    public class ProductWithCompositeEnumIntKeysController : ODataController
    {
        private static readonly List<ProductWithCompositeEnumIntKey> DataCompositeKey;

        static ProductWithCompositeEnumIntKeysController()
        {
            DataCompositeKey = new List<ProductWithCompositeEnumIntKey>()
            {
                {
                    new ProductWithCompositeEnumIntKey
                    {
                        EnumValue = MyEnum.ValueOne,
                        Id = 1,
                        Name = "ValueOneName",
                        Price = 101
                    }
                },
                {
                    new ProductWithCompositeEnumIntKey
                    {
                        EnumValue = MyEnum.ValueTwo,
                        Id = 2,
                        Name = "ValueTwoName",
                        Price = 102
                    }
                }
            };
        }

        /// <summary>
        /// Query products
        /// </summary>
        [EnableQuery]
        public IQueryable<ProductWithCompositeEnumIntKey> Get()
        {
            return DataCompositeKey.AsQueryable();
        }

        /// <summary>
        /// Query products by keys
        /// </summary>
        /// <param name="keyenumValue">key enum value</param>
        /// <param name="keyid">key id</param>
        /// <returns>composite enum-int key model</returns>
        [EnableQuery]
        public IHttpActionResult Get([FromODataUri]MyEnum keyenumValue, [FromODataUri]int keyid)
        {
            return Ok(DataCompositeKey
                        .Where(x => x.EnumValue == keyenumValue 
                                    && x.Id == keyid));
        }
    }
}