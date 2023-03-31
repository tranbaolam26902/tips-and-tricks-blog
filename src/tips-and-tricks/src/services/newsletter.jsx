import axios from 'axios';

export async function subscribe(email) {
	const response = await axios.post(
		`https://localhost:7157/api/subscribers/${encodeURIComponent(
			email,
		)}/subscribe`,
	);

	const data = response.data;
	if (data.isSuccess) return true;
	else return data.errors;
}
