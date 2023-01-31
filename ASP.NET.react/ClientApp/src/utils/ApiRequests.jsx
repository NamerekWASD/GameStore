

export async function CheckAuthenticated() {
    const response = await fetch(`account`, {
        method: "GET",
        headers: {
            "Accept": "application/json",
        }
    })

    if(!response.ok){
        return false
    }

    const responseResult = await response.json()
    console.log(responseResult);
    return responseResult
};

export const logout = async () => {
    const response = await fetch('account/logout')
    if (response.ok) {
        CheckAuthenticated()
    }
}
