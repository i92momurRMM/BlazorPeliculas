﻿using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Helpers
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityUser> claimsFactory;
        private readonly UserManager<IdentityUser> userManager;

        public IdentityProfileService(IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            UserManager<IdentityUser> userManager)
        {
            this.claimsFactory = claimsFactory;
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var usuarioId = context.Subject.GetSubjectId();
            var usuario = await userManager.FindByIdAsync(usuarioId);
            // Get claims
            var claimsPrincipal = await claimsFactory.CreateAsync(usuario);
            var claims = claimsPrincipal.Claims.ToList();


            //Map because Blazor components need ClaimTypes.Role Type and claimsPrincipal is JwtClaimTypes.Role
            var claimsMapeados = new List<Claim>();

            foreach (var claim in claims)
            {
                if (claim.Type == JwtClaimTypes.Role)
                {
                    claimsMapeados.Add(new Claim(ClaimTypes.Role, claim.Value));
                }
            }
            // Added mapped claims
            claims.AddRange(claimsMapeados);

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var usuarioId = context.Subject.GetSubjectId();
            var usuario = await userManager.FindByIdAsync(usuarioId);
            context.IsActive = usuario != null;
        }
    }
}
