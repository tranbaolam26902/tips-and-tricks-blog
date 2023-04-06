import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function subscribe(email) {
	const response = await axios.post(
		`${API_URL}/api/subscribers/${encodeURIComponent(email)}/subscribe`,
	);

	const data = response.data;
	if (data.isSuccess) return true;
	else return data.errors;
}
