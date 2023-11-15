using Matkakertomus.Shared;

namespace Matkakertomus.Server.Models
{
	public static class DtoMapper
	{
		public static DestinationDto ToDto(this Destination destination)
		{
			return new DestinationDto
			{
				DestinationName = destination.DestinationName,
				DestinationId = destination.DestinationId,
				Country = destination.Country,
				Area = destination.Area,
				Description = destination.Description,
				Image = destination.Image,
			};
		}

		public static Destination ToDestination(this DestinationDto destination)
		{
			return new Destination
			{
				DestinationName = destination.DestinationName,
				DestinationId = destination.DestinationId,
				Country = destination.Country,
				Area = destination.Area,
				Description = destination.Description,
				Image = destination.Image,
			};
		}

		public static StoryDto ToDto(this Story story)
		{
			return new StoryDto
			{
				StoryId = story.StoryId,
				Date = story.Date,
				Text = story.Text,
				TripId = story.TripId,
				DestinationId = story.DestinationId,
			};
		}
		public static TripDto ToDto(this Trip trip)
		{
			return new TripDto
			{
				TripId = trip.TripId,
				Title = trip.Title,
				StartDate = trip.StartDate,
				EndDate = trip.EndDate,
				Private = trip.Private,
				TravellerId = trip.TravellerId,
			};
		}

		public static Trip ToTrip(this TripDto trip)
		{
			return new Trip
			{
				Title = trip.Title,
				TripId = trip.TripId,
				StartDate = trip.StartDate,
				EndDate = trip.EndDate,
				Private = trip.Private,
				TravellerId = trip.TravellerId,
			};
		}

		public static ProfileDto ToDto(this Traveller traveller)
		{
			return new ProfileDto
			{
				TravellerId = traveller.TravellerId,
				FirstName = traveller.FirstName,
				LastName = traveller.LastName,
				Username = traveller.Username,
				Area = traveller.Area,
				Description = traveller.Description,
				Image = traveller.Image,
				Email = traveller.Email,
			};
		}

		public static Traveller ToTraveller(this ProfileDto traveller)
		{
			return new Traveller
			{
				TravellerId = traveller.TravellerId,
				FirstName = traveller.FirstName,
				LastName = traveller.LastName,
				Username = traveller.Username,
				Area = traveller.Area,
				Description = traveller.Description,
				Image = traveller.Image,
				Email = traveller.Email,
			};
		}

		public static ImageDto ToDto(this Picture image)
		{
			return new ImageDto
			{
				ImageId = image.ImageId,
				Image = image.Image,
				StoryId = image.StoryId,
			};
		}

		public static Picture ToPicture(this ImageDto image)
		{
			return new Picture
			{
				ImageId = image.ImageId,
				Image = image.Image,
				StoryId = image.StoryId,
			};
		}
	}
}

