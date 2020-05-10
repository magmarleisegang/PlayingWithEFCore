CREATE VIEW PawtionResults AS
select Name, BagSize, PPP, AddedDate
from DogFoods 
inner join Pawtions on DogFoods.Id = Pawtions.DogFoodId