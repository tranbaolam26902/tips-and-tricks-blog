import axios from 'axios';

export async function subscribe(email) {
	const { data } = await axios.post(
		`${
			process.env.REACT_APP_API_ENDPOINT
		}/api/subscribers/${encodeURIComponent(email)}/subscribe`,
	);

	if (data.isSuccess) return true;
	else return data.errors;
}
