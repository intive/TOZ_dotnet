using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public class ProposalsController : Controller
    {
        private readonly IProposalsManagementService _proposalsManagementService;
        private readonly IBackendErrorsService _backendErrorsService;
        private readonly IStringLocalizer<ProposalsController> _stringLocalizer;
        private readonly AppSettings _appSettings;

        public ProposalsController(IProposalsManagementService proposalsManagementService, IBackendErrorsService backendErrorsService, IStringLocalizer<ProposalsController> localizer, IOptions<AppSettings> appSettings)
        {
            _proposalsManagementService = proposalsManagementService;
            _backendErrorsService = backendErrorsService;
            _stringLocalizer = localizer;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var proposals = await _proposalsManagementService.GetAllProposals(cancellationToken);
            return View(proposals.OrderByDescending(prop => prop.CreationTime));
        }

        public IActionResult Add()
        {
            return PartialView(new Proposal());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Proposal proposal, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(proposal);
            }

            if (await _proposalsManagementService.CreateProposal(proposal, cancellationToken))
            {
                return Json(new {success = true});
            }

            var overallError = _backendErrorsService.UpdateModelState(ModelState);
            if (!string.IsNullOrEmpty(overallError))
            {
                ViewData["UnhandledError"] = overallError;
            }
            return PartialView(proposal);
        }

        public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            return PartialView("Edit", await _proposalsManagementService.GetProposal(id, cancellationToken));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Proposal proposal, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(proposal);
            }

            if (await _proposalsManagementService.UpdateProposal(proposal, cancellationToken))
            {
                return Json(new { success = true });
            }

            var overallError = _backendErrorsService.UpdateModelState(ModelState);
            if (!string.IsNullOrEmpty(overallError))
            {
                ViewData["UnhandledError"] = overallError;
            }
            return PartialView(proposal);
        }

        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var proposal = await _proposalsManagementService.GetProposal(id, cancellationToken);
            if (proposal != null)
            {
                await _proposalsManagementService.DeleteProposal(proposal, cancellationToken);
            }
            return View("Index");
        }

    }
}
