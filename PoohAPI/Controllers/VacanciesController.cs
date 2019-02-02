﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("vacancies")]
    public class VacanciesController : Controller
    {
        private readonly IVacancyReadService vacancyReadService;
        private readonly IVacancyCommandService vacancyCommandService;

        public VacanciesController(IVacancyReadService vacancyReadService, IVacancyCommandService vacancyCommandService)
        {
            this.vacancyReadService = vacancyReadService;
            this.vacancyCommandService = vacancyCommandService;
        }

        /// <summary>
        /// Gets a list of all vacancies
        /// </summary>
        /// <remarks>Returns Vacancies or BaseVacancies. The model used is determined by detailedVacancy.</remarks>
        /// <param name="maxCount">The max amount of vacancies to return, defaults to 5</param>
        /// <param name="offset">The number of vacancies to skip</param>
        /// <param name="additionalLocationSearchTerms">Searchwords to narrow the resultsets, comma seperated list</param>
        /// <param name="educationId">The id of the education</param>
        /// <param name="educationalAttainmentId">The id of the educationlevel (HBO, WO, Univerity, etc.)</param>
        /// <param name="internshipType">The type of intership</param>
        /// <param name="languageId">id of the language to get vacancies for</param>
        /// <param name="cityName">The city name where the vacancy is located in</param>
        /// <param name="countryName">The coutry name where the vacancy is located in</param>
        /// <param name="locationRange">The range where the vacancies must be retrieved within</param>
        /// <param name="isActive">Wether the vacancies need to be active or not</param>

        /// <returns>A list of all vacancies</returns>
        /// <response code="200">Returns the list of vacancies</response>
        /// <response code="404">If no vacancies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vacancy>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5, [FromQuery]int offset = 0, [FromQuery]string additionalLocationSearchTerms = null, [FromQuery]int? educationId = null, [FromQuery]int? educationalAttainmentId = null, [FromQuery]IntershipType? internshipType = null, [FromQuery]int? languageId = null, [FromQuery]string cityName = null, [FromQuery]string countryName = null, [FromQuery]int? locationRange = null, [FromQuery]int isActive = 1)
        {
            if (maxCount < 0 || maxCount > 100)
            {
                return BadRequest("MaxCount should be between 1 and 100");
            }

            if (offset < 0)
            {
                return BadRequest("Offset should be 0 or larger");
            }

            IEnumerable<Vacancy> vacancies = this.vacancyReadService.GetListVacancies(maxCount, offset, additionalLocationSearchTerms, educationId, educationalAttainmentId, internshipType, languageId, cityName, countryName, locationRange, isActive);

            if (!(vacancies is null))
            {
                return Ok(vacancies);
            }
            else
            {
                return NotFound("No vacancies were found");
            }
        }

        /// <summary>
        /// Gets a specific vacancy by Id
        /// </summary>
        /// <param name="id">The Id of the vacancy to retrieve</param>
        /// <returns>One specific vacancy</returns>
        /// <response code="200">Returns the requested vacancy</response>
        /// <response code="404">If the specified vacancy was not found</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Vacancy), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            Vacancy vacancy = this.vacancyReadService.GetVacancyById(id);

            if (vacancy != null)
            {
                return Ok(vacancy);
            }
            else
            {
                return NotFound("vacancy not found.");
            }
        }
    }
}