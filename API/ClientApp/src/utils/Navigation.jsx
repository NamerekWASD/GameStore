import { toast } from "react-toastify";
import { requestIsInRole } from "./ApiRequests";
import { AppPaths } from "./AppPaths";
import { MANAGER } from "./Constants";

export const navigateToDetails = (game, navigate) => {
    const searchParams = new URLSearchParams({ "id": game.id, "title": game.title });
    navigate(AppPaths.gameDetails + '?' + searchParams.toString());
};

export const navigateToManager = (navigate) => {
    if(verify(MANAGER)){
        navigate(AppPaths.manager)
    }
}

export const verify = async (role) => {
    const response = await requestIsInRole(role);
    if(response.redirected){
        toast.error(await response.text());
        return false;
    }
    return true;
} 