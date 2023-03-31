import axios from 'axios';

export async function getCategories() {
	const response = await axios.get(
		'https://localhost:7157/api/categories?PageSize=10&PageNumber=1',
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getFeaturedPosts(limit) {
	const response = await axios.get(
		`https://localhost:7157/api/posts/featured/${limit}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getRandomPosts(limit) {
	const response = await axios.get(
		`https://localhost:7157/api/posts/random/${limit}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
