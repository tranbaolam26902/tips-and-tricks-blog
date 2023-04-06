import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function getCategories() {
	const { data } = await axios.get(
		`${API_URL}/categories?PageSize=10&PageNumber=1`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getFeaturedPosts(limit) {
	const { data } = await axios.get(`${API_URL}/posts/featured/${limit}`);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getRandomPosts(limit) {
	const { data } = await axios.get(`${API_URL}/posts/random/${limit}`);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getTagCloud() {
	const { data } = await axios.get(
		`${API_URL}/tags?PageSize=100&PageNumber=1`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getTopAuthors(limit) {
	const { data } = await axios.get(`${API_URL}/authors/best/${limit}`);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getArchives(limit) {
	const { data } = await axios.get(`${API_URL}/posts/archive/${limit}`);

	if (data.isSuccess) return data.result;
	else return null;
}
