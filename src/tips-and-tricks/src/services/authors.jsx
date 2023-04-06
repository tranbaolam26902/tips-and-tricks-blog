import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getAuthorBySlug(slug) {
	const response = await axios.get(`${API_URL}/api/authors/${slug}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getAuthors() {
	const response = await axios.get(
		`${API_URL}/api/authors?PageSize=1000&PageNumber=1`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
