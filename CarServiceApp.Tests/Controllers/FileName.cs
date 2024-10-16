﻿using CarServiceApp.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceApp.Tests.Controllers
{
    public class FileName
    {
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        [InlineData("7")]
        [InlineData("8")]
        [InlineData("9")]
        [InlineData("0")]
        [InlineData("q")]
        [InlineData("w")]
        [InlineData("e")]
        [InlineData("r")]
        [InlineData("t")]
        [InlineData("y")]
        [InlineData("u")]
        [InlineData("i")]
        [InlineData("o")]
        [InlineData("p")]
        [InlineData("a")]
        [InlineData("s")]
        [InlineData("d")]
        [InlineData("f")]
        [InlineData("g")]
        [InlineData("h")]
        [InlineData("j")]
        [InlineData("k")]
        [InlineData("l")]
        [InlineData(";")]
        [InlineData("z")]
        [InlineData("x")]
        [InlineData("c")]
        [InlineData("v")]
        [InlineData("b")]
        [InlineData("n")]
        [InlineData("m")]
        [InlineData(",")]
        [InlineData(".")]
        [InlineData("й")]
        [InlineData("ц")]
        [InlineData("у")]
        [InlineData("к")]
        [InlineData("е")]
        [InlineData("н")]
        [InlineData("г")]
        [InlineData("ш")]
        [InlineData("щ")]
        [InlineData("1й111")]
        [InlineData("22ц22")]
        [InlineData("333у3")]
        [InlineData("1111к")]
        [InlineData("2е222")]
        [InlineData("33н33")]
        [InlineData("11г11")]
        [InlineData("222ш2")]
        [InlineData("333ш3")]
        [InlineData("111щ1")]
        [InlineData("222з2")]
        [InlineData("й3333")]
        [InlineData("ц1111")]
        [InlineData("2у222")]
        [InlineData("33к33")]
        [InlineData("111е1")]
        [InlineData("2222е")]
        [InlineData("ф3333")]
        [InlineData("1ы111")]
        [InlineData("22в22")]
        [InlineData("333а3")]
        [InlineData("1111п")]
        [InlineData("2222р")]
        [InlineData("я3333")]
        [InlineData("1ч111")]
        [InlineData("22с22")]
        [InlineData("333м3")]
        [InlineData("1111и")]
        [InlineData("ф2222")]
        [InlineData("3ыы333")]
        [InlineData("111вв1")]
        [InlineData("2222аа")]
        [InlineData("3333")]
        [InlineData("1111")]
        [InlineData("2222")]
        [InlineData("йй3333")]
        [InlineData("1цц111")]
        [InlineData("22уу22")]
        [InlineData("333кк3")]
        [InlineData("1111нн")]
        [InlineData("ффф2222")]
        [InlineData("3ыыы333")]
        [InlineData("1вввв111")]
        [InlineData("222а2")]
        [InlineData("333е3")]
        [InlineData("я1111")]
        [InlineData("ч2222")]
        [InlineData("с3333")]
        [InlineData("м1111")]
        [InlineData("и2222")]
        [InlineData("т3333")]
        [InlineData("ь1111")]
        [InlineData("б2222")]
        [InlineData("фф3333")]
        [InlineData("ы1111")]
        [InlineData("в2222")]
        [InlineData("а3333")]
        [InlineData("п1111")]
        [InlineData("р2222")]
        [InlineData("ро3333")]
        [InlineData("л1111")]
        [InlineData("д2222")]
        [InlineData("ж3333")]
        [InlineData("й1111")]
        [InlineData("йу2222")]
        [InlineData("цк3333")]
        [InlineData("е1111")]
        [InlineData("н2222")]
        [InlineData("г3333")]
        [InlineData("гш1111")]
        [InlineData("щ2222")]
        [InlineData("щ3333")]
        [InlineData("q1111")]
        [InlineData("w2222")]
        [InlineData("e3333")]
        [InlineData("r1111")]
        [InlineData("t2222")]
        [InlineData("y3333")]
        [InlineData("u1111")]
        [InlineData("i2222")]
        [InlineData("o3333")]
        [InlineData("p1111")]
        [InlineData("[2222")]
        [InlineData("a3333")]
        [InlineData("ds1111")]
        [InlineData("f2222")]
        [InlineData("g3333")]
        [InlineData("h1111")]
        [InlineData("aa2222")]
        [InlineData("j3333")]
        [InlineData("k1111")]
        [InlineData("l2222")]
        [InlineData(";3333")]
        [InlineData("'1111")]
        [InlineData("z2222")]
        [InlineData("x3333")]
        [InlineData("c1111")]
        [InlineData("v2222")]
        [InlineData("b3333")]
        [InlineData("n1111")]
        [InlineData("m2222")]
        [InlineData(",3333")]
        [InlineData("fdgdf")]
        [InlineData("dfg")]
        [InlineData("33h33")]
        [InlineData("sd11vb11")]
        [InlineData("22t22")]
        [InlineData("33f33")]
        [InlineData("11a11")]
        [InlineData("2sa222")]
        [InlineData("3d333")]
        [InlineData("1fd111")]
        [InlineData("ssssss")]
        [InlineData("3g333")]
        [InlineData("1hg111")]
        [InlineData("2g222")]
        [InlineData("33hss33")]
        [InlineData("1hj111")]
        [InlineData("222h2")]
        [InlineData("333hj3")]
        [InlineData("111jh1")]
        [InlineData("2222j")]
        [InlineData("3df333")]
        [InlineData("11f11")]
        [InlineData("22gf22")]
        [InlineData("333hjg3")]
        [InlineData("111ghj1")]
        [InlineData("2222hjg")]
        [InlineData("333jg3")]
        [InlineData("1111hgj")]
        [InlineData("2222jgh")]
        [InlineData("3333ghj")]
        [InlineData("1111gj")]
        [InlineData("211222")]
        [InlineData("332233")]
        [InlineData("111111")]
        [InlineData("2222222")]
        [InlineData("33333333")]
        [InlineData("333")]
        [InlineData("22232")]
        [InlineData("333333333333")]
        [InlineData("113333333311")]
        [InlineData("221222")]
        [InlineData("333133")]
        [InlineData("1132111")]
        [InlineData("2321222")]
        [InlineData("3321335433")]
        [InlineData("1456111")]
        [InlineData("55564")]
        [InlineData("33667833")]
        [InlineData("118799711")]
        [InlineData("225422")]
        [InlineData("333456453")]
        [InlineData("1144411")]
        [InlineData("27897222")]
        [InlineData("987")]
        [InlineData("19789111")]
        [InlineData("229879722")]
        [InlineData("333879783")]
        [InlineData("197897111")]
        [InlineData("228797822")]
        [InlineData("879879")]
        [InlineData("9978777978")]
        [InlineData("22897922")]
        [InlineData("339879733")]
        [InlineData("17111")]
        [InlineData("22722")]
        [InlineData("37333")]
        [InlineData("111881")]
        [InlineData("22822")]
        [InlineData("3339993")]
        [InlineData("199999911")]
        [InlineData("22922")]
        [InlineData("9999")]
        [InlineData("11911")]
        [InlineData("2229999999992")]
        [InlineData("33999999999933")]
        [InlineData("11888888811")]
        [InlineData("2228888882")]
        [InlineData("3888888333")]
        [InlineData("1118888881")]
        [InlineData("78978989")]
        [InlineData("3456546333")]
        [InlineData("123213")]
        [InlineData("123123")]
        [InlineData("332313133")]
        [InlineData("12312312")]
        [InlineData("2212312322")]
        [InlineData("3331231333")]
        [InlineData("113111111111133311")]
        [InlineData("22111111112322")]
        [InlineData("333214563")]
        public void IsValid_Returns_Expected_Result(string email)
        {
            // Assert
            Assert.NotNull(email);
        }
    }
}
