Access - Control - Allow - Origin: *


const addButton = document.getElementById("addRecipe");

addButton.addEventListener("click", async () => {
    const recipeName = document.getElementById("recipeName").value;
    const calories = document.getElementById("calories").value;
    const ingredients = document.getElementById("ingredients").value;
    const portionWeight = document.getElementById("portionWeight").value;

    const recipe = {
        RecipeName: recipeName,
        CalorieValue: calories,
        IngredientsNeeded: ingredients,
        DishWeight: portionWeight
    };

    try {
        const response = await fetch('https://localhost:7114/Recipes', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(recipe)
        });

        if (response.ok) {
            const responseData = await response.json();
            // Действия после успешной отправки данных
        } else {
            console.log('Ошибка при выполнении запроса:', response.status);
        }
    } catch (error) {
        console.log('Ошибка при выполнении запроса:', error);
    }
});
