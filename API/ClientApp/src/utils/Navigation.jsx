import { AppPaths } from "./AppPaths";

export const navigateToDetails = (game, navigate) => {
    const searchParams = new URLSearchParams({ "id": game.id, "title": game.title });
    navigate(AppPaths.gameDetails + '?' + searchParams.toString());
};