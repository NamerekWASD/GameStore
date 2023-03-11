import { AppPaths } from "./AppPaths";

export async function CheckAuthenticated() {
    const response = await fetch(`api/account`, {
        method: "GET",
        headers: {
            "Accept": "application/json",
        }
    });

    if (!response.ok) {
        return false;
    }

    const responseResult = await response.json();
    return responseResult;
};

export const loadUserData = async (navigate, useNavigate, waitResult) => {
    const response = await fetch('api/account/data', {
        method: "GET",
        headers: {
            "Accept": "application/json",
        }
    })
    if (useNavigate && response.redirected) {
        navigate(AppPaths.login + '?' + new URLSearchParams([['returnURL', (await response.json()).ReturnUrl]]));
        return;
    }
    if(waitResult){
        return await response.json();
    }
    return response;
}

export const logout = async () => {
    const response = await fetch('api/account/logout');
    if (response.ok) {
        CheckAuthenticated();
    }
}
export async function loadGames(detailed) {

    const requestInfo = `api/game` + (detailed ? '/detailed' : '');
    const requestInit = {
        method: 'GET',
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
    };

    const response = await fetch(requestInfo, requestInit);
    const responseBody = await response.json();
    return responseBody;

};
export async function loadGame(id) {
    const requestInfo = "api/game/" + id;
    const requestInit = {
        method: 'GET',
    };

    const response = await fetch(requestInfo, requestInit)
    const responseBody = await response.json();
    return responseBody;
}

export async function loadGenres() {
    const requestInfo = "api/game/genres";
    const requestInit = {
        method: 'GET',
    };

    const response = await fetch(requestInfo, requestInit)
    const responseBody = await response.json();
    return responseBody;
}

export async function loadGamesByGenre(id) {
    const requestInfo = "api/game/genre/" + id;
    const requestInit = {
        method: 'GET',
    };

    const response = await fetch(requestInfo, requestInit)
    const responseBody = await response.json();
    return responseBody;
}